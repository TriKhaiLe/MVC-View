using System;
using System.Collections.Generic;

namespace NET_API.Models;

public partial class Account
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
