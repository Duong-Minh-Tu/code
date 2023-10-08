using SellingBook.DbContexts;
using SellingBook.Dtos.BooksDto;
using SellingBook.Dtos.CartsDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using AutoMapper;
using SellingBook.Exceptions;
using System.Linq;

namespace SellingBook.Services.Implements
{
    public class CartServices : ICartServices
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public CartServices(ILogger<CartServices> logger, ApplicationDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        // tìm giỏ hàng
        public Cart Find()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var cartFind = _dbContext.carts.FirstOrDefault(s => s.IdUser == userid);
            var cartDetailQuery = _dbContext.cartDetails.AsQueryable().Where(o => o.IdCart == cartFind.Id).ToList();
            List<CartDetail> ListcartDetails = new List<CartDetail>();
            if (cartFind == null)
            {
                throw new UserFriendlyException("khong tim thay gio hang");
            }
            foreach (var cartbook in cartDetailQuery)
            {
                var cartDetailFind = _dbContext.cartDetails.FirstOrDefault(o => o.IdCart == cartFind.Id && o.Id == cartbook.Id);
                var bookfind = _dbContext.books.FirstOrDefault(x => x.Id == cartDetailFind.IdBook);
                cartDetailFind.book = bookfind;
                cartDetailFind.Price = (float)(bookfind.Price * cartDetailFind.Quantity);
                ListcartDetails.Add(cartDetailFind);
            }
            var totalPrice = ListcartDetails.Sum(x => x.Price);

            cartFind.cartDetails = ListcartDetails;
            cartFind.IdUser = userid;
            cartFind.Total = totalPrice;
            return cartFind;
        }

        public int Deleted(int id)
        {
            var cartQuery = _dbContext.carts.AsQueryable();
            var cartFind = cartQuery.FirstOrDefault(s => s.Id == id);
            if (cartFind == null)
            {
                throw new UserFriendlyException("khong tim thay gio hang");
            }
            _dbContext.carts.Remove(cartFind);
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// xóa item trong cart
        /// </summary>
        /// <param name="listIdBook"></param>
        /// <returns></returns>
        public int DeletedItemCart(List<int> listIdBook)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var cartFind = _dbContext.carts.FirstOrDefault(c => c.IdUser == userid);
            foreach(var item in listIdBook)
            {
                var itemDetailFind = _dbContext.cartDetails.FirstOrDefault(d => d.IdCart == cartFind.Id && d.IdBook == item);
                if (itemDetailFind == null)
                {
                    return 0;
                }
                _dbContext.cartDetails.Remove(itemDetailFind);
            }
            _dbContext.SaveChanges();
            return 0;
        }

        /// <summary>
        /// thêm item vào cart
        /// </summary>
        /// <param name="add"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateCart(List<ListBookCart> add)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            var cartQuery = _dbContext.carts.AsQueryable();
            var bookQuery = _dbContext.books.AsQueryable();
            var cardDetailQuery = _dbContext.cartDetails.AsQueryable();

            var cartFind = cartQuery.FirstOrDefault(s => s.IdUser == userid);
            if (cartFind == null)
            {
                throw new UserFriendlyException("khong tim thay gio hang");
            }
            foreach (var item in add)
            {
                var cardDeitalFind = cardDetailQuery.FirstOrDefault(o => o.IdCart == cartFind.Id && o.IdBook == item.IdBook);
                var bookFind = bookQuery.FirstOrDefault(x => x.Id == item.IdBook);
                if (bookFind == null)
                {
                    throw new UserFriendlyException("khong tim thay quyen sach nay");
                }
                if (cardDeitalFind != null)
                {
                    cardDeitalFind.Quantity = item.Quantity;
                }
                else
                {
                    _dbContext.cartDetails.Add(new CartDetail()
                    {
                        IdCart = cartFind.Id,
                        Price = bookFind.Price,
                        IdBook = bookFind.Id,
                        Quantity = item.Quantity,
                    });
                }
            }   
            _dbContext.SaveChanges();
        }
    }
}
