using SallesWebMvc.Data;
using SallesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SallesWebMvc.Services.Exceptions;

namespace SallesWebMvc.Services
{
    public class SellerService
    {
        private readonly SallesWebMvcContext _context;

        public SellerService(SallesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            _context.Add(obj);

            // este é o método que de fato executado o comando SQL no banco,
            // por isso é nele que se colocar o recurso Async
            await _context.SaveChangesAsync(); 
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            // EAGER LOADING: include (entityFrameWork) - para adicionar um join no select do seller com department
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);  
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateException e)
            {

                throw new IntegrityException("Can't delete seller because he/she has sales!");
            }
            
        }

        public async Task UpdateAsync (Seller obj)
        {
            if(! await _context.Seller.AnyAsync(x => x.Id == obj.Id))
            {
                throw new DllNotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
