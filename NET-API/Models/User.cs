using System;
using System.Collections.Generic;

namespace NET_API.Models;

public partial class User
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Fullname { get; set; }
}
