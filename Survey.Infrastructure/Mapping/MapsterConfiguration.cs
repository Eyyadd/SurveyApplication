using Mapster;
using Survey.Domain.Entities;
using Survey.Infrastructure.DTOs.Poll.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Mapping
{
    public static class MapsterConfiguration
    {
        public static void Configure()
        {
            TypeAdapterConfig<PollResponse, Poll>.NewConfig()
                .Ignore(x => x.CreatedAt)
                .Ignore(x => x.CreatedBy)
                .Ignore(x => x.CreatedByUserId);
        }
    }
}
