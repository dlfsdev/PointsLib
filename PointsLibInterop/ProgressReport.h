#pragma once

#include "Point2d.h"

namespace PointsLibInterop
{

public ref class ProgressReport sealed
{
public:
    property System::Nullable<double> Progress;
    property System::Collections::Generic::IEnumerable<Point2d^>^ ProvisionalAnswer;
};

}