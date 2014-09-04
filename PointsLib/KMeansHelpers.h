// Standalone functions that need to be avaialble to more than one k-means class

#pragma once

#include <vector>

namespace PointsLib { struct Point; }

namespace PointsLib { namespace KMeansHelpers
{
    // Given k, a vector of points, and the list of clusters they are assigned to, compute each cluster's centroid
    std::vector<Point> ComputeCentroids(
        int k,
        const std::vector<Point>& points,
        const std::vector<int>& assignments);
}}