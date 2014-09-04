#include "stdafx.h"
#include "PointExtensions.h"

using namespace System::Collections::Generic;
using namespace System::Linq;

namespace PointsLibInterop { namespace Private {

PointsLib::Point MakeNativePoint(Point2d^ point)
{
    return PointsLib::Point(point->X, point->Y);
}

std::vector<PointsLib::Point> MakeNativePoints(IEnumerable<Point2d^>^ points)
{
    std::vector<PointsLib::Point> nativePoints;
    nativePoints.reserve(Enumerable::Count(points));

    for each(Point2d^ point in points)
        nativePoints.push_back(MakeNativePoint(point));

    return nativePoints;
}

Point2d^ MakeManagedPoint(const PointsLib::Point& point)
{
    return gcnew Point2d(point.x, point.y);
}

IEnumerable<Point2d^>^ MakeManagedPoints(const std::vector<PointsLib::Point>& points)
{
    auto managedPoints = gcnew List<Point2d^>((int)points.size());
    for(const auto& nativePoint : points)
        managedPoints->Add(MakeManagedPoint(nativePoint));
    return managedPoints;
}

} }