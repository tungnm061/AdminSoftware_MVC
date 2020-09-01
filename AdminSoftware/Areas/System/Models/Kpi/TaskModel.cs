using System;
using System.ComponentModel.DataAnnotations;
using Entity.Kpi;

namespace AdminSoftware.Areas.System.Models.Kpi
{
    public class TaskModel
    {
        public string TaskId { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskCode { get; set; }

        [Required]
        [StringLength(255)]
        public string TaskName { get; set; }

        [Required]
        public byte CalcType { get; set; }
        [Required]
        public int WorkPointConfigId { get; set; }
        [Required]
        public decimal UsefulHours { get; set; }

        public int? CategoryKpiId { get; set; }

        [Required]
        public bool Frequent { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsSystem { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        public int CreateBy { get; set; }
        [Required]
        [StringLength(500)]
        public string GroupName { get; set; }
        public Task ToObject()
        {
            return new Task
            {
                TaskId = TaskId,
                TaskCode = TaskCode,
                TaskName = TaskName,
                CalcType = CalcType,
                WorkPointConfigId = WorkPointConfigId,
                UsefulHours = UsefulHours,
                CategoryKpiId = CategoryKpiId,
                Frequent = Frequent,
                Description = Description,
                IsSystem = IsSystem,
                CreateDate = CreateDate,
                CreateBy = CreateBy,
                GroupName = GroupName
            };
        }
    }
}