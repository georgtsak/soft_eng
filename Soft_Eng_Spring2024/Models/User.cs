namespace Soft_Eng_Spring2024.Models;
using System;

public class User
{
	public int Id { get; set; }
	public string firstName { get; set; }
	public string lastName { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }

	public User()
	{
	}
}
