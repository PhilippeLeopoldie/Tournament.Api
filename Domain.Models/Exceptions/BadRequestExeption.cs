using Domain.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions;

public abstract class BadRequestException : Exception
{
    public string Title { get; set; }
    protected BadRequestException(string message, string title = "Bad request") : base(message)
    {
        Title = title;
    }
}

public class BadRequest : BadRequestException
{
    public BadRequest(int id) : base($"id: '{id}' do not match id from body")
    {
    }
    public BadRequest() : base($"No patchDocument")
    {
    }
}