using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SkinTime.DAL.Entities;
using SkinTime.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SkinTime.BLL.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //public Task<IQueryable<T>> GetAll()
        //{// hàm này là lấy tất cả các entity của thực thể nhưng không truy vấn ngay lâpj tức => mục đích là để sử dụng cho việc truy vấn sau này
        //    return _context.Set<T>();
        //}
        public async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {// thằng này đơn giản là thêm entity vào db context
            await _context.Set<T>().AddAsync(entity);
        }

        public int Count()
        {// thàng này là đếm số lượng  
            return _context.Set<T>().Count();
        }

        public async Task<int> CountAsync()
        {// đếm nhưng có thểm bất đồng bộ
            return await _context.Set<T>().CountAsync();
        }

        public void Delete(T entity)
        {// xóa entity dựa vào entity đó => khác với thằng xóa ở dưới là xóa trực tiếp entity ko cần phải tìm => cải thiện hiệu xuất
            _context.Set<T>().Remove(entity);
        }

        public void Delete(Guid id)
        {// cũng là xóa nhưng phải thêm 1 bước ttifm => hiệu xuât chậm 
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }

        public bool Exists(Guid id)
        {// Kiểm tra xem là thhanwgf này có tồn tại trong db hay không dựa vào id
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {// cũng là tra nhưng kiểm tra xem có có điều kiện => và thực thể này phải thỏa mản điêuf kiện predicate
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public T? Find(Expression<Func<T, bool>> match)
        {// hàm này là tìm kiếm entity dựa vào điều kiện match => nếu tìm thấy thì trả về entity đó còn không thì trả về null
            return _context.Set<T>().SingleOrDefault(match);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> match)
        {// cũng là tìm nhưng bất đồng bộ
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }



        public T? GetById(Guid id)
        {// hàm này là là lấy entity theo id và có thể sửa đổi entity đó và sử dụng savechanges để lưu thay đổi
            return _context.Set<T>().Find(id);
        }

        public T? GetByIdAsDetached(Guid id)
        {// còn hàm này thì khác với hàm trên ở chỗ nó sẽ lấy và chỉ xem không có sửa đổi=> mục đích để sử dụng là để Db context ko còn dính dán gì sau khi lấy thực thể => bản chất là sao khi lấy thực thể thì có thì thằng DB Con text sẽ ăn ram khá nhiều => hiệu suât chậm  
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public async Task<T?> GetEntityByIdAsync(Guid id)
        {// hàm này khá giống với trên nhưng thêm bất đồng bộ 
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {// hàm này là lấy tất cả các entity của thực thể nhưng bất đồng bộ. Thực thi ngay lập tức khi gọi phương thức.
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync()
        {// như trên nhưng lấy dựa vào id đê lấy tất cả
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByConditionAsync(
    Expression<Func<T, bool>> filter,
    Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null
)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            return await query.FirstOrDefaultAsync(filter);
        }


        public async Task<IEnumerable<T>> ListAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        )
        {// thằng này dùng để lọc và sắp xếp sau khi lấy 1 list entity từ db => Tham số filter: Đây là một biểu thức lambda cho phép bạn chỉ định điều kiện lọc cho truy vấn (ví dụ: e => e.IsActive == true).
         // Tham số orderBy: Đây là một hàm chức năng cho phép bạn chỉ định cách sắp xếp kết quả, ví dụ: query => query.OrderBy(e => e.Name).
            IQueryable<T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> ListAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProperties = null
        )
        {// cũng như thằng trên nhưng thêm includeProperties để include các thực thể khác 
            IQueryable <T> query = _context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }

        public void Update(T entity)
        {// thằng này đơn giản  là cập nhật entity
            _context.Set<T>().Update(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.AddRangeAsync(entities);
        }
    }
}
