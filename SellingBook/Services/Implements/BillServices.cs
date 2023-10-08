using SellingBook.Constants;
using SellingBook.DbContexts;
using SellingBook.Dtos.BillDto;
using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Dtos.Discounts;
using SellingBook.Dtos.ReviewDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;
using AutoMapper;

namespace SellingBook.Services.Implements
{
    public class BillServices : IBillServices
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BillServices(ILogger<BillServices> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public void CreateBill(List<int> ListIdBook, string name, int payment, int delivery, int addressId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            // tìm kiếm giỏ hàng
            var cartFind = _dbContext.carts.FirstOrDefault(x => x.IdUser == userId);
            var bill = _dbContext.bills.Add(new Bill());
            _dbContext.SaveChanges();
            List<BillDetail> listBillDetail = new List<BillDetail>();
            // với mỗi id trong giỏ hàng được tích
            foreach (var IdBook in ListIdBook)
            {
                var cartDetailFind = _dbContext.cartDetails.FirstOrDefault(x => x.IdCart == cartFind.Id && x.IdBook == IdBook);
                // add billDetail khi thanh toán giỏ hàng
                var bookFind = _dbContext.books.FirstOrDefault(x => x.Id == IdBook);
                float price;
                if(bookFind.DiscountPercent != null)
                {
                    price = (float)(bookFind.Price * bookFind.DiscountPercent);
                }
                var discountFind = _dbContext.discounts.FirstOrDefault(o => o.Name == name);
                if (discountFind != null)
                {
                    price = (float)(bookFind.Price - bookFind.Price * discountFind.DiscountPercent);
                    bookFind.DiscountPercent = discountFind.DiscountPercent;
                }
                else
                {
                    price = bookFind.Price;
                }
                // add billDetail
                var resultBill = _dbContext.billDetails.Add(new BillDetail()
                {
                    IdBook = IdBook,
                    BillId = bill.Entity.Id,
                    BookName = bookFind.BookName,
                    Quantity = cartDetailFind.Quantity,
                    TotalPrice = price * cartDetailFind.Quantity,
                    CreateDate = DateTime.Now,
                    IsReview = "N",
                    CreateBy = username,
                });
                if(bookFind.TotalSales == null)
                {
                    bookFind.TotalSales = cartDetailFind.Quantity;
                }
                else
                {
                    bookFind.TotalSales += cartDetailFind.Quantity;
                }
                _dbContext.SaveChanges();
                // tìm sách để xóa
                var cartBookFind = _dbContext.cartDetails.FirstOrDefault(x => x.IdBook == IdBook);
                // tìm BillDetail để add vào list 
                var billDetailFind = _dbContext.billDetails.FirstOrDefault(b => b.Id == resultBill.Entity.Id);
                billDetailFind.book = bookFind;
                listBillDetail.Add(billDetailFind);
               
                // xóa những quyển sách được thanh toán trong giỏ hàng
                _dbContext.cartDetails.Remove(cartBookFind);
                _dbContext.SaveChanges();
            }
            _dbContext.SaveChanges();
            // add detailBill và tổng của giỏ hàng
            var billDetailFinds = _mapper.Map<List<BillDetailsDto>>(listBillDetail);
            var billSum = billDetailFinds.Select(x => x.TotalPrice).Sum();
            bill.Entity.Status = 1;
            bill.Entity.AddressId = addressId;
            if (delivery == BookDelivery.DeliverySave){ bill.Entity.TotalPrice = billSum + BookDelivery.DeliverySaveValue;}
            else if(delivery == BookDelivery.DeliveryFast) { bill.Entity.TotalPrice = billSum + BookDelivery.DeliveryFastValue; }
            else if(delivery == BookDelivery.DeliveryExpress) { bill.Entity.TotalPrice = billSum + BookDelivery.DeliveryExpress; }
            var x = bill.Entity.Id;
            bill.Entity.Delivery = delivery;
            bill.Entity.Payments = payment;
            bill.Entity.IdCart = cartFind.Id;
            bill.Entity.IdUser = userId;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// lấy tất cả bill của user đăng nhập
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<BillDto> Find(int Status)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var billQuery = _dbContext.bills.AsQueryable().Where(o => o.IdUser == userId && o.Status == Status);
            List<BillDto> ListBill = new List<BillDto>();
            if (billQuery == null)
            {
                throw new Exception("khong tim thay hoa don");
            }
            else
            {
                foreach (var billItem in billQuery)
                {
                    var billDetailQuery = _dbContext.billDetails.AsQueryable().Where(s => s.BillId == billItem.Id);
                    var billDetail = _mapper.Map<List<BillDetailsDto>>(billDetailQuery);
                    byte[] image;
                    foreach (var item in billDetail)
                    {
                        //var billDetailFind = _dbContext.billDetails.FirstOrDefault(s => s.BillId == billItem.Id);
                        var bookFind = _dbContext.books.FirstOrDefault(b => b.Id == item.IdBook);
                        item.TotalPrice = item.Quantity * bookFind.Price;
                        //image = bookFind.Image;
                        //item.book = bookFind;
                    }
                    billItem.BillDetail = billDetail;
                    var sum = billItem.BillDetail.Sum(b => b.TotalPrice);
                    billItem.TotalPrice = sum;
                    var billMap = new BillDto()
                    {
                        billDetails = billItem.BillDetail,
                        TotalPrice = billItem.TotalPrice,
                    };
                    ListBill.Add(billMap);
                }
            }
            return ListBill;
        }


        /// <summary>
        /// tìm bill theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Bill FindById(int id)
        {
            var billQuery = _dbContext.bills.AsQueryable();
            var billFind = billQuery.FirstOrDefault(s => s.Id == id);
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            if (billFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoa don");
            }
            foreach(var item in billDetailQuery)
            {
                var billDetailFind = _dbContext.billDetails.FirstOrDefault(s => s.BillId == id);
                var bookFind = _dbContext.books.FirstOrDefault(b => b.Id == billDetailFind.IdBook);
                billDetailFind.TotalPrice = billDetailFind.Quantity * bookFind.Price;
                billDetailFind.book = bookFind;
            }
            billFind.TotalPrice = billFind.BillDetail.Sum(b => b.TotalPrice);
            return billFind;
        }

        public int Deleted(int id)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            var billDetailFind = billDetailQuery.FirstOrDefault(s => s.Id == id);
            if (billDetailFind == null)
            {
                throw new UserFriendlyException("khong tim thay chi tiet hoa don");
            }
            _dbContext.billDetails.Remove(billDetailFind);
            _dbContext.SaveChanges();
            return 0;
        }

        public void UpdateBill(CreateBillDto input, int id)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            var billDetailFind = billDetailQuery.FirstOrDefault(s => s.Id == id);
            if (billDetailFind == null)
            {
                throw new UserFriendlyException("khong tim thay chi tiet hoa don");
            }
            billDetailFind.TotalPrice = input.TotalPrice;
            _dbContext.SaveChanges();
        }

        public PageResultDto<List<BillDto>> FindAll(FilterDto input)
        {
            var billQuery = _dbContext.bills.AsQueryable();
            //if (input.Keyword != null)
            //{
            //    billDetailQuery = billDetailQuery.Where(s => s.ListIdBill != null && s.ListIdBill.Contains(input.Keyword));
            //}
            List<BillDto> ListBill = new List<BillDto>();
            List<BillDetail> ListBillDetail = new List<BillDetail>();
            foreach (var billItem in billQuery)
            {
                var billDetailQuery = _dbContext.bills.AsQueryable();
                if (billItem == null)
                {
                    throw new UserFriendlyException("khong tim thay hoa don");
                }
                foreach (var item in billDetailQuery)
                {
                    var billDetailFind = _dbContext.billDetails.FirstOrDefault(s => s.Id == item.Id && s.BillId == billItem.Id);
                    var bookFind = _dbContext.books.FirstOrDefault(b => b.Id == billDetailFind.IdBook);
                    billDetailFind.book = bookFind;
                    billDetailFind.TotalPrice = billDetailFind.Quantity * bookFind.Price;
                    var billDetails = _mapper.Map<BillDetailsDto>(billDetailFind);
                    billItem.BillDetail.Add(billDetails);

                    var sum = billItem.BillDetail.Sum(b => b.TotalPrice);
                    billItem.TotalPrice = sum;
                }
                var billMap = new BillDto()
                {
                    billDetails = billItem.BillDetail,
                    TotalPrice = billItem.TotalPrice,
                };
                ListBill.Add(billMap);
            }
            if (input.PageSize != -1)
            {
                ListBill.Skip(input.Skip).Take(input.PageSize);
            }
            else
            {
                ListBill.Skip(input.PageSize * (input.PageIndex - 1)).Take(input.PageSize);
            }
            int totalItem = ListBill.Count();
            return new PageResultDto<List<BillDto>>
            {
                Item = ListBill,
                TotalItem = totalItem
            };
        }

        public void Status(int idBill)
        {
            var billFind = _dbContext.bills.FirstOrDefault(b => b.Id == idBill);
            if (billFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoa don");
            }

            if (billFind != null && billFind.Status == 1) 
            {
                billFind.Status = 2;
            }
            else if (billFind != null && billFind.Status == 2)
            {
                billFind.Status = 3;
            }
            else if (billFind != null && billFind.Status == 3)
            {
                billFind.Status = 4;
            }
        }

        public void CreateDiscount(CreateDiscountDto input)
        {
            var discoundFind = _dbContext.discounts.Any(d => d.Name == input.Name);
            if(discoundFind)
            {
                throw new UserFriendlyException("đã trùng mã giảm giá vui lòng nhập mã mới");
            }    
            _dbContext.discounts.Add(new Discount()
            {
                Name= input.Name,
                DiscountPercent = input.DiscountPercent,
                Active= "y",
            });
            _dbContext.SaveChanges();
        }

        public void UpdateDiscount(UpdateDiscountDto input)
        {
            var discoundFind = _dbContext.discounts.FirstOrDefault(o => o.Id == input.Id || o.Name == input.Name);
            discoundFind.DiscountPercent = input.DiscountPercent;
            _dbContext.SaveChanges();
        }

        public void DiscounStatus(UpdateDiscountDto input)
        {
            var discoundFind = _dbContext.discounts.FirstOrDefault(o => o.Id == input.Id || o.Name == input.Name);
            if(discoundFind == null)
            {
                throw new UserFriendlyException("không tìm thấy mã giảm giá");
            }    
            if(discoundFind != null && discoundFind.Active == "y")
            {
                discoundFind.Active = "n";
            }    
            else if(discoundFind != null && discoundFind.Active == "n")
            {
                discoundFind.Active = "y";
            }    
            _dbContext.SaveChanges();
        }
    }
}
