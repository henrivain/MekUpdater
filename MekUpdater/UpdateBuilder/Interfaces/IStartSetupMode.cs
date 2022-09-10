using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MekUpdater.UpdateBuilder.Interfaces;

public interface IStartSetupMode
{
    ICanFinishUpdate IsTrue();
    ICanFinishUpdate IsFalse();
}
