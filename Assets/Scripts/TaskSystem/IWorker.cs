using UnityEngine;
using System;

namespace TaskSystem
{
    public interface IWorker
    {
        void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
    }
}