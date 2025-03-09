using fitapp.Data;
using fitapp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fitapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly FitnessAppContext _context;

        public NotificationsController(FitnessAppContext context)
        {
            _context = context;
        }

        // ✅ **1. جلب جميع الإشعارات**
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
        {
            return await _context.Notifications.OrderByDescending(n => n.CreatedAt).ToListAsync();
        }

        // ✅ **2. جلب إشعار واحد حسب ID**
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound();
            return notification;
        }

        // ✅ **3. إضافة إشعار جديد**
        [HttpPost]
        public async Task<ActionResult<Notification>> CreateNotification(Notification notification)
        {
            if (notification == null)
                return BadRequest("Invalid notification data");

            notification.CreatedAt = DateTime.UtcNow;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNotification), new { id = notification.Id }, notification);
        }

        // ✅ **4. تحديث بيانات إشعار**
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, Notification updatedNotification)
        {
            if (id != updatedNotification.Id)
                return BadRequest("ID mismatch");

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound();

            notification.Title = updatedNotification.Title;
            notification.Description = updatedNotification.Description;
            notification.ImageUrl = updatedNotification.ImageUrl;
            notification.IsFollowed = updatedNotification.IsFollowed;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ **5. حذف إشعار**
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ **6. تغيير حالة المتابعة للإشعار**
        [HttpPatch("{id}/follow")]
        public async Task<IActionResult> ToggleFollowNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                return NotFound();

            notification.IsFollowed = !notification.IsFollowed;
            await _context.SaveChangesAsync();
            return Ok(new { notification.Id, notification.IsFollowed });
        }
    }
}
