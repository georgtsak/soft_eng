namespace Soft_Eng_Spring2024.Models;
using System;

public class User
{
	public int Id { get; set; }
	public string Firstname { get; set; }
	public string Lastname { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Salt { get; set; }
	public int Role { get; set; }

}
