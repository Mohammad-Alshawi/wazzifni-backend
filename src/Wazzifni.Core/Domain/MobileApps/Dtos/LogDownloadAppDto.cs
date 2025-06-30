using System;
using System.Collections.Generic;

namespace Wazzifni.Domain.MobileApps.Dtos
{
    public class LogDownloadAppDto
    {
        public DateTime DateTime { get; set; }
        public int DownloadCount { get; set; }
    }

    public class ResultDownloadCountDto
    {
        public List<LogDownloadAppDto> downloadCountDtos { get; set; }

    }

}
