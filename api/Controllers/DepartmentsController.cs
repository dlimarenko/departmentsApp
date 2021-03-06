﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        DepartmentContext db;
        public DepartmentsController(DepartmentContext context)
        {
            db = context;
        }

        [HttpGet]
        public DepartmentsPage Get([FromQuery] Paginator paginator)
        {
            if (paginator.PageSize == 0)
                paginator = new Paginator()
                {
                    Length = db.Departments.Count(),
                    PageIndex = 0,
                    PageSize = 5
                };

            var departments = db.Departments
                .Skip(paginator.PageSize * paginator.PageIndex)
                .Take(paginator.PageSize)
                .ToList();

            return new DepartmentsPage(departments, paginator);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Department department)
        {
            if (ModelState.IsValid)
            {
                if (db.Departments.Where(e => e.Name == department.Name).Count() == 0)
                {
                    department.isEmpty = true;
                    db.Departments.Add(department);
                    db.SaveChanges();
                    return Ok(department);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Department department)
        {
            if (ModelState.IsValid)
            {
                if (db.Departments.Where(e => e.Name == department.Name).Count() == 0)
                {
                    db.Update(department);
                    db.SaveChanges();
                    return Ok(department);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Department department = db.Departments.FirstOrDefault(x => x.Id == id);
            if (department != null)
            {
                db.Departments.Remove(department);
                db.SaveChanges();
            }
            return Ok(department);
        }
    }
}
