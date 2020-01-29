using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Rivals
{
	public static class RivalsTask
	{
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var queue = new Queue<OwnedLocation>();
			for (int i = 0; i < map.Players.Length; i++)
				queue.Enqueue(new OwnedLocation(i, map.Players[i], 0));

			var way = new HashSet<Point>();

			while (queue.Count != 0)
			{
				var own = queue.Dequeue();
				way.Add(own.Location);

				foreach (var move in own.Location.NearbyMoves(map).Where(p => !way.Contains(p)))
				{
					if (queue.Any(l => l.Location == move)) continue;
					var next = new OwnedLocation(own.Owner, move, own.Distance + 1);
					queue.Enqueue(next);
				}
				yield return new OwnedLocation(own.Owner, own.Location, own.Distance);
			}

			yield break;
		}

		public static IEnumerable<Point> NearbyMoves(this Point point, Map map, int min = -1, int max = 1)
		{
			var range = Enumerable.Range(min, max - min + 1);

			return range
				.SelectMany(x => range.Select(y => new Point(point.X + x, point.Y + y)))
				.Where(p => p != point && ((p.X - point.X) == 0 || (p.Y - point.Y) == 0) &&
					   map.InBounds(p) && map.Maze[p.X, p.Y] == MapCell.Empty);
		}
	}
}
