#pragma once

#include "IPointsAlgorithmObserver.h"

#include <functional>

namespace PointsLib
{

namespace Private
{
    // Wraps a call to an algorithm's main solve function in the required
    // OnStarting/OnComplete/OnFailure observer notifications
    template<typename T>
    auto RunObservableAlgorithm(const T& solve, IPointsAlgorithmObserver& observer) -> decltype(solve())
    {
        try
        {
            observer.OnStarting();
            auto answer = solve();
            observer.OnComplete(answer);
            return answer;
        }
        catch(...)
        {
            try
            {
                observer.OnFailure();
            }
            catch(...)
            {
                // we're already handling an exception; ignore subsequent ones
            }

            throw;
        }
    }
}

}