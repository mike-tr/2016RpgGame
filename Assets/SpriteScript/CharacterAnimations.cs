using UnityEngine;
using System.Collections;

public class CharacterAnimations {
    public CharacterDirection Cd;
    public CharacterAnimation Ca;

    public static int MaxColumn = 21;
    public static int MaxRow = 13;

    public static Vector3 GetDirection(CharacterDirection a)
    {
        switch(a)
        {
            case CharacterDirection.right:
                return Vector2.right;
            case CharacterDirection.left:
                return Vector2.left;
            case CharacterDirection.front:
                return Vector2.down;
            case CharacterDirection.back:
                return Vector2.up;

        }
        return Vector2.zero;
    }

    public static int GetAnimationLenght(CharacterAnimation a)
    {
        switch(a)
        {
            case CharacterAnimation.SpellCast:
                return 7;
            case CharacterAnimation.Thrust:
                return 8;
            case CharacterAnimation.Walk:
                return 9;
            case CharacterAnimation.Slash:
                return 6;
            case CharacterAnimation.Shoot:
                return 13;
            case CharacterAnimation.Hurt:
                return 6;
            case CharacterAnimation.idle:
                return 1;
        }
        return 0;
    }
}

public enum CharacterDirection
{
    back = 0,
    left = 1,
    front = 2,
    right = 3
}

public enum WalkDirections
{
    up = 1,
    down = 2,
    right = 3,
    left = 6,
    right_up = right + up,
    right_down = right + down,
    left_up = left + up,
    left_down = left + down,
    none = 0
}

public enum CharacterAnimation
{
    idle = -1,
    SpellCast,
    Thrust,
    Walk,
    Slash,
    Shoot,
    Hurt,
}
