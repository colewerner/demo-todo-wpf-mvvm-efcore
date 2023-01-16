using System;
using System.Collections.Generic;

namespace Todo.Database.Models;

public partial class Todo
{
    public int TodoId { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }
}
