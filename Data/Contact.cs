﻿using DaisyStudy.Data.Enums;

namespace DaisyStudy.Data;

public class Contact
{
    public int ContactID { set; get; }
    public string? CustomerName { set; get; }
    public string? Email { set; get; }
    public string? PhoneNumber { set; get; }
    public string? Message { set; get; }
    public DateTime DateTimeCreated { set; get; }
    public Status Status { set; get; }
}