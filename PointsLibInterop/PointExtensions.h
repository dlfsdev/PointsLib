#pragma once

#include "Point2d.h"

#include "../PointsLib/Point.h"

namespace PointsLibInterop { namespace Private
{

PointsLib::Point MakeNativePoint(Point2d^ point);
std::vector<PointsLib::Point> MakeNativePoints(System::Collections::Generic::IEnumerable<Point2d^>^ points);

Point2d^ MakeManagedPoint(const PointsLib::Point& point);
System::Collections::Generic::IEnumerable<Point2d^>^ MakeManagedPoints(const std::vector<PointsLib::Point>& points);

} }