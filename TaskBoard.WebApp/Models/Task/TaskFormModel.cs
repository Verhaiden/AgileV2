using TaskBoard.Data;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskBoard.WebApp.Models.Task
{
    using static DataConstants;
    public class TaskFormModel
    {
        [Required]
        [StringLength(MaxTaskTitle, MinimumLength = MinTaskTitle,
            ErrorMessage = "Tytuł powinien mieć conajmniej {2} znaki długości.")]
        public string Title { get; set; }

        [Required]
        [StringLength(MaxTaskDescription, MinimumLength = MinTaskDescription, 
            ErrorMessage = "Opis powinien mieć co najmniej {2} znaki długości.")]
        public string Description { get; set; }

        [Display(Name = "Board")]
        public int BoardId { get; set; }

        public IEnumerable<TaskBoardModel> Boards { get; set; }
    }
}
