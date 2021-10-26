    using AutoMapper;
using leave_management.Contracts;
using leave_management.Data;
using leave_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    namespace leave_management.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        public LeaveAllocationController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<Employee> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;

        }
        // GET: LeaveAllocationController
        public async Task<ActionResult> Index()
        {
            var leavetype = await _unitOfWork.LeaveTypes.FindAll();
            var mappedLeaveTypes = _mapper.Map<List<LeaveType>, List<LeaveTypeVM>>(leavetype.ToList());
            var model = new CreateLeaveAllocationVM
            {
                LeaveTypes = mappedLeaveTypes,
                NumberUpdated = 0
            };
            return View(model);
        }
        public async Task<ActionResult> SetLeave(int id)
        {
            var leaveType = await _unitOfWork.LeaveTypes.Find(q => q.Id == id);
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var period = DateTime.Now.Year;
                foreach (var emp in employees)
                {
                    if (await _unitOfWork.LeaveAllocations.isExist(q => q.EmployeeId == emp.Id
                                                                        && q.LeaveTypeId == id
                                                                        && q.Period == period))
                        continue;
                    var allocation = new LeaveAllocationVM
                    {
                        DateCreated = DateTime.Now,
                        EmployeeId = emp.Id,
                        LeaveTypeId = id,
                        NumberOfDays = leaveType.DefaultDays,
                        Period = DateTime.Now.Year
                    };
                    var leaveAllocation = _mapper.Map<LeaveAllocation>(allocation);
                    await _unitOfWork.LeaveAllocations.Create(leaveAllocation);
                    await _unitOfWork.Save();
                }
                return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> ListEmployees()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            var model = _mapper.Map<List<EmployeeVM>>(employees);
            return View(model);
        }

        // GET: LeaveAllocationController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var employee = _mapper.Map<EmployeeVM>(await _userManager.FindByIdAsync(id));
            var period = DateTime.Now.Year;
            var allocations = _mapper.Map<List<LeaveAllocationVM>>(await _unitOfWork.LeaveAllocations.FindAll(q => q.EmployeeId == id 
                                                                                                                                            && q.Period == period,
                                                                                                                includes: q => q.Include(x => x.LeaveType)));
            var model = new ViewAllocationsVM
            {
                Employee = employee,
                LeaveAllocations = allocations
            };
            return View(model);
        }

        // GET: LeaveAllocationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LeaveAllocationController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var leaveAllocation = await _unitOfWork.LeaveAllocations.Find(q => q.Id == id,
                                                                            includes: q => q.Include(x => x.Employee).Include(x => x.LeaveType));
            var model = _mapper.Map<EditLeaveAllocationsVM>(leaveAllocation);

            return View(model);
        }

        // POST: LeaveAllocationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditLeaveAllocationsVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var record = await _unitOfWork.LeaveAllocations.Find(q => q.Id == model.Id);
                record.NumberOfDays = model.NumberOfDays;

                _unitOfWork.LeaveAllocations.Update(record);
                await _unitOfWork.Save();
                return RedirectToAction(nameof(Details), new { id = model.EmployeeId });
            }
            catch
            {
                return View(model);
            }
        }

        // GET: LeaveAllocationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LeaveAllocationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
