#pragma once

#include "IPointsAlgorithm.h"

#include <vector>

namespace PointsLib
{

class QuickHullSolver : public IPointsAlgorithm
{
public:
	static QuickHullSolver* Create();
	virtual ~QuickHullSolver();

    // Compute the set of vertexes that define a polygon that is the convex hull of a collection of points.
    // Specifically does NOT return all the points in the input that lie on the convex hull.
    virtual std::vector<Point> Solve(const std::vector<Point>& points) const = 0;

protected:
	QuickHullSolver();
	void operator=(const QuickHullSolver&) = delete;
};

}