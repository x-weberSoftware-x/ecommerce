using System;

namespace Core.Entities;

public class BaseEntity
{
    //calling this Id tells entity framework to use this as the primary key
    public int Id { get; set; } 
    
}
