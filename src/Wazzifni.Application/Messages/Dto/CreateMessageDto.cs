using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wazzifni.Messages.Dto
{
    public class CreateMessageDto
    {
        public long? UserReceiverId { get; set; }
        public string Content { get; set; }
    }
}
