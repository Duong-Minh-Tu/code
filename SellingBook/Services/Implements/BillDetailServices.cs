using SellingBook.DbContexts;
using SellingBook.Dtos.BillDto;
using SellingBook.Dtos.BillDto.BillDetailDto;
using SellingBook.Entities;
using SellingBook.Exceptions;
using SellingBook.Page;
using SellingBook.Services.Interfaces;
using SellingBook.Utils;

namespace SellingBook.Services.Implements
{
    public class BillDetailServices : IBillDetailServices
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        public BillDetailServices(ILogger<BillDetailServices> logger, ApplicationDbContext dbContext, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public BillDetail FindById(int id)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            var billDetailFind = billDetailQuery.FirstOrDefault(s => s.Id == id);
            if (billDetailFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoa don");
            }
            return billDetailFind;
        }

        public int Deleted(int id)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            var billDetailFind = billDetailQuery.FirstOrDefault(s => s.Id == id);
            if (billDetailFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoa don");
            }
            _dbContext.billDetails.Remove(billDetailFind);
            _dbContext.SaveChanges();
            return 0;
        }
        public void UpdateBillDetail(CreateBillDetailDto input, int id)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            var billDetailFind = billDetailQuery.FirstOrDefault(s => s.Id == id);
            if (billDetailFind == null)
            {
                throw new UserFriendlyException("khong tim thay hoa don");
            }
            billDetailFind.IdBook = input.IdBook;
            billDetailFind.TotalPrice = input.TotalPrice;
            billDetailFind.BookName = input.BookName;
            billDetailFind.Quantity = input.Quatity;
            billDetailFind.CreateDate = input.CreateDate;
            billDetailFind.CreateBy = input.CreateBy;
            _dbContext.SaveChanges();
        }
        public PageResultDto<List<BillDetail>> FindAll(FilterDto input)
        {
            var billDetailQuery = _dbContext.billDetails.AsQueryable();
            if (input.Keyword != null)
            {
                billDetailQuery = billDetailQuery.Where(s => s.BookName != null && s.BookName.Contains(input.Keyword));
            }
            int totalItem = billDetailQuery.Count();

            billDetailQuery = billDetailQuery.Skip(input.PageSize * (input.PageIndex - 1)).Take(input.PageSize);
            return new PageResultDto<List<BillDetail>>
            {
                Item = billDetailQuery.ToList(),
                TotalItem = totalItem
            };
        }


    }
}
