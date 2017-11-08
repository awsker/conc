using System.Collections.Generic;

namespace conc.game.math
{
    public interface IPolygon
    {
        IEnumerable<Line> GetLines();
    }
}
