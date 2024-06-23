﻿namespace Soft_Eng_Spring2024.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class User
{
	public int Id { get; set; }
	public string Firstname { get; set; } = "";
	public string Lastname { get; set; } = "";
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = "";
	[DataType(DataType.Password)]
	public string Password { get; set; } = "";
    public string Salt { get; set; } = "";
	public int Role { get; set; } = 0;

}
