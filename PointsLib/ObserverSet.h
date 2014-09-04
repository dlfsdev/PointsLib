#pragma once

#include <algorithm>
#include <memory>
#include <mutex>
#include <vector>

namespace Util
{

template<typename ObserverType>
class ObserverSet
{
public:
    void AddObserver(const std::weak_ptr<ObserverType>& observer)
    {
        std::lock_guard<std::mutex> lock(m_lock);
        RemoveDeadObservers();
        m_observers.push_back(observer);
    }

    void RemoveObserver(const ObserverType& observer)
    {
        std::lock_guard<std::mutex> lock(m_lock);

        RemoveDeadObservers();
        
        auto itFirstMatch = std::find_if(m_observers.begin(), m_observers.end(),
            [&](const std::weak_ptr<ObserverType>& weakObserver)
            {
                auto strongObserver = weakObserver.lock();
                return strongObserver != nullptr && strongObserver.get() == &observer;
            });

        if(itFirstMatch != m_observers.end())
            m_observers.erase(itFirstMatch);
    }

    template<typename FunctorType>
    void ForEachObserver(const FunctorType& f) const
    {
        for(const auto& observer : GetCopyOfLiveObserverList())
            f(*observer);
    }

private:
    void RemoveDeadObservers() const
    {
        auto itNewEnd = std::remove_if(m_observers.begin(), m_observers.end(),
            [&](const std::weak_ptr<ObserverType>& weakObserver) { return weakObserver.lock() == nullptr; });
        m_observers.erase(itNewEnd, m_observers.end());
    }

    std::vector<std::shared_ptr<ObserverType>> GetCopyOfLiveObserverList() const
    {
        std::lock_guard<std::mutex> lock(m_lock);
        
        RemoveDeadObservers();
        
        std::vector<std::shared_ptr<ObserverType>> copy;
        for(const auto& weakObserver : m_observers)
        {
            auto observer = weakObserver.lock();
            if(observer != nullptr) // could have just died on another thread
                copy.push_back(observer);
        }

        return copy;
    }

private:
    mutable std::mutex m_lock;
    mutable std::vector<std::weak_ptr<ObserverType>> m_observers;
};

}