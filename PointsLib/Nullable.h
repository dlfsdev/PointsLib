#pragma once

#include <exception>
#include <memory>
#include <type_traits>

namespace Util
{

// boost::optional is better, but this will do the job
// Requires T to be copy construcible
template<typename T>
struct Nullable
{
    static_assert(std::is_copy_constructible<T>::value, "T must be copy constructible");

    Nullable()
        : m_value()
    {
    }

    Nullable(const T& value)
    {
        *this = value;
    }

    Nullable(const Nullable<T>& other)
    {
        *this = other;
    }

    Nullable<T>& operator=(const Nullable<T>& other)
    {
        if(other.m_value != nullptr)
            *this = *other.m_value;
        else
            m_value.reset();

        return *this;
    }

    Nullable<T>& operator=(const T& value)
    {
        m_value.reset(new T(value));
        return *this;
    }

    bool HasValue() const
    {
        return m_value != nullptr;
    }

    const T& Value() const
    {
        if(m_value == nullptr)
            throw std::exception("Attempt to access a null value");
        return *m_value;
    }

    void Reset()
    {
        m_value.reset();
    }

private:
    std::unique_ptr<T> m_value;
};

}