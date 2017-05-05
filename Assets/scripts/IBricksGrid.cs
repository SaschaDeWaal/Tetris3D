using UnityEngine;
using System.Collections;
using Sascha;

namespace Sascha {
    public interface IBricksGrid {

        void Clear();
        void Merge(IBricksGrid grid);
        Brick GetBrick(IntVector3 pos);
        void AddBrick(IntVector3 pos, Brick brick);
        void RemoveBrick(IntVector3 pos);
        bool BrickExsist(IntVector3 pos);
        IntVector3 GetGridSize();
        IntVector3 GetPosition();
        IntVector3 GetRotation();
        Brick[] GetBricks();

    }
}