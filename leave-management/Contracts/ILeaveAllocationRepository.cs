﻿using leave_management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Contracts
{
    public interface ILeaveAllocationRepository : IRepositoryBase<LeaveAllocation>
    {
        bool CheckAllocation(int leaveTypeId, string employeeid);
        ICollection<LeaveAllocation> GetLeaveAllocationsByEmployee(string id);
        public LeaveAllocation GetLeaveAllocationsByEmployeeAndType(string id, int leaveTypeId);
    }
}