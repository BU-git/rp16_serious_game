using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using DAL;
using Domain.Entities;
using Interfaces;
using Microsoft.AspNet.Authorization;
using WebUI.ViewModels.Appointments;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace WebUI.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private IDal _dal;

        public AppointmentsController(IDal dal)
        {
            _dal = dal;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            //Get appointments for current user
            var apps = (await _dal.GetUserAppointments(HttpContext.User.GetUserId())).OrderBy(a => a.Start);//.Where(a=>a.Start >= DateTime.Now);
            var avm = new List<AppointmentViewModel>();
            foreach (var appointment in apps)
            {
                var appointmentView = new AppointmentViewModel()
                {
                    Id = appointment.Id,
                    Description = appointment.Description,
                    Start = appointment.Start,
                    End = appointment.End
                };
                avm.Add(appointmentView);
            }
            return View(avm);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var appointment = await _dal.GetAppointmentById(id.Value);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            var appointmentView = new AppointmentViewModel()
            {
                Id = appointment.Id,
                Description = appointment.Description,
                Start = appointment.Start,
                End = appointment.End,
                Users = appointment.Appointment_Users.Select(a => a.User).ToList()
            };
            return View(appointmentView);
        }

        // GET: Appointments/Create
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _dal.GetUsers();
            ViewBag.Creator = HttpContext.User.GetUserId();
            return View();
        }

        // POST: Appointments/Create
        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel appointmentView, string users)
        {
            if (ModelState.IsValid)
            {
                var appointment = new Appointment()
                {
                    Description = appointmentView.Description,
                    Start = appointmentView.Start,
                    End = appointmentView.Start.Date + appointmentView.End.TimeOfDay
                };
                var attendees = users.Split(',');
                var val = await _dal.ValidateAppointment(appointment.Start, appointment.End, attendees);

                if (val == null)
                {
                    await _dal.CreateAppointment(appointment, HttpContext.User.GetUserId(), attendees);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"{val.User.Name} already has appointment {val.Appointment.Start.ToShortDateString()} from {val.Appointment.Start.ToShortTimeString()} to {val.Appointment.End.ToShortTimeString()}");
                }
            }
            ViewBag.Users = await _dal.GetUsers();
            ViewBag.Creator = HttpContext.User.GetUserId();
            return View(appointmentView);
        }

        // GET: Appointments/Edit/5
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var appointment = await _dal.GetAppointmentById(id.Value);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            var appointmentView = new AppointmentViewModel()
            {
                Id = appointment.Id,
                Description = appointment.Description,
                Start = appointment.Start,
                End = appointment.End,
                Users = appointment.Appointment_Users.Select(a => a.User).ToList()
            };

            ViewBag.Users = await _dal.GetUsers();
            return View(appointmentView);
        }

        // POST: Appointments/Edit/5
        [HttpPost]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentViewModel appointmentView, string users)
        {
            if (ModelState.IsValid)
            {
                var appointment = await _dal.GetAppointmentById(appointmentView.Id);
                appointment.Description = appointmentView.Description;
                appointment.Start = appointmentView.Start;
                appointment.End = appointmentView.Start.Date + appointmentView.End.TimeOfDay;

                var attendees = users.Split(',');
                var val = await _dal.ValidateAppointment(appointment.Start, appointment.End, attendees);

                if (val == null)
                {
                    await _dal.EditAppointment(appointment, attendees);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", $"{val.User.Name} already has appointment {val.Appointment.Start.ToShortDateString()} from {val.Appointment.Start.ToShortTimeString()} to {val.Appointment.End.ToShortTimeString()}");
                }
            }
            ViewBag.Users = await _dal.GetUsers();
            return View(appointmentView);
        }

        // GET: Appointments/Delete/5
        [ActionName("Delete")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var appointment = await _dal.GetAppointmentById(id.Value);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            var appointmentView = new AppointmentViewModel()
            {
                Id = appointment.Id,
                Description = appointment.Description,
                Start = appointment.Start,
                End = appointment.End,
                Users = appointment.Appointment_Users.Select(a => a.User).ToList()
            };
            return View(appointmentView);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Coach")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _dal.DeleteAppointment(id);
            return RedirectToAction("Index");
        }
    }
}
