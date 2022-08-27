using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoWebApi.DataAccess
{
    [Table("ToDoItems")]
    public class ToDoItemModel
    {
        [Key]
        public Guid Id { get; private set; }
        public string ToDoName { get; private set; }
        public bool Completed { get; private set; }
        public DateTime? CompletedOn { get; private set; }
        public DateTime CreatedOn { get; private set; }

        public ToDoItemModel(string toDoName)
            : this(Guid.NewGuid(), toDoName, false, null, DateTime.UtcNow)
        {
        }

        private ToDoItemModel(Guid id, string toDoName, bool completed, DateTime? completedOn, DateTime createdOn)
        {
            Id = id;
            ToDoName = toDoName;
            Completed = completed;
            CompletedOn = completedOn;
            CreatedOn = createdOn;
        }

        public void SetCompleted()
        {
            Completed = true;
            CompletedOn = DateTime.UtcNow;
        }

        public void SetIncomplete()
        {
            Completed = false;
            CompletedOn = null;
        }
    }
}