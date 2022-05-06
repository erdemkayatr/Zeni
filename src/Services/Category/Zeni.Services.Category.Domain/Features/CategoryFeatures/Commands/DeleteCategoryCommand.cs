using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.Commands
{
    public class DeleteCategoryCommand : Command<bool>
    {
        public Guid Id { get; set; }

        public DeleteCategoryCommand(Guid id)
        {
            Id = id;
        }
    }
}
