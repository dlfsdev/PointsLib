#pragma once

#include <memory>
#include <vector>

namespace PointsLib { struct IPointsAlgorithmObserver; }
namespace PointsLib { struct Point; }

namespace PointsLib
{

struct IPointsAlgorithm
{
    virtual ~IPointsAlgorithm() {}

    virtual std::vector<Point> Solve(const std::vector<Point>& points) const = 0;

    // Indicate whether the algorithm's progress reports will include a fraction complete 
    virtual bool ReportsDefiniteProgress() const = 0;

    // Observer management.
    // Weak registration; it is not necessary to unregister objects before they are destroyed.
    virtual void AddObserver(const std::weak_ptr<IPointsAlgorithmObserver>& observer) const = 0;
    virtual void RemoveObserver(const IPointsAlgorithmObserver& observer) const = 0;
};

}