#include "stdafx.h"
#include "KMeansHelpers.h"

#include "Point.h"

#include <assert.h>

namespace PointsLib { namespace KMeansHelpers
{

std::vector<Point> ComputeCentroids(
    int k,
    const std::vector<Point>& points,
    const std::vector<int>& assignments)
{
    assert(k > 0);
    assert(!assignments.empty());
    assert(assignments.size() == points.size());

    std::vector<Point> centroids(k);
    std::vector<size_t> counts(k);

    for(size_t i = 0; i < points.size(); ++i)
    {
        const Point& point = points[i];
        const int assignedCentroidIndex = assignments[i];
        assert(assignedCentroidIndex >= 0 && assignedCentroidIndex < k);
        ++counts[assignedCentroidIndex];
        Point& assignedCentroid = centroids[assignedCentroidIndex];
        assignedCentroid.x += point.x;
        assignedCentroid.y += point.y;
    }

    for(size_t i = 0; i < centroids.size(); ++i)
    {
        Point& centroid = centroids[i];
        const size_t count = counts[i];
        if(count == 0)
            continue; // TODO
        centroid.x /= count;
        centroid.y /= count;
    }

    return centroids;
}

}}