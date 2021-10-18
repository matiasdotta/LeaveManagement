using leave_management.Contracts;
using leave_management.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Repository
{
    public class LeaveAllocationRepository : ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CheckAllocation(int leaveTypeId, string employeeid)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await FindAll();
            return leaveAllocations.Where(q => q.EmployeeId == employeeid && q.LeaveTypeId == leaveTypeId && q.Period == period).Any(); 
        }

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToListAsync<LeaveAllocation>();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            return await _db.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public ICollection<LeaveType> GetEmployeesByLeaveAllocation(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<LeaveAllocation>> GetLeaveAllocationsByEmployee(string id)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await FindAll();
            return leaveAllocations.Where(q => q.EmployeeId == id && q.Period == period).ToList();
        }

        public async Task<LeaveAllocation> GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId)
        {
            var period = DateTime.Now.Year;
            var leaveAllocations = await FindAll();
            return leaveAllocations.FirstOrDefault(q => q.EmployeeId == id && q.Period == period && q.LeaveTypeId == leaveTypeId);
        }

        public async Task<bool> isExist(int id)
        {
            var exists = await _db.LeaveAllocations.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Update(entity);
            return await Save();
        }

    }
}
