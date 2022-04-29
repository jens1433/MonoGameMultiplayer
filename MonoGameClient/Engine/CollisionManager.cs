using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class CollisionManager
{
    private Dictionary<GameObject, List<GameObject>> _collisions = new Dictionary<GameObject, List<GameObject>>();
    private List<GameObject> calledEnter, calledStay, calledExit;

    public CollisionManager()
    {
        calledEnter = new List<GameObject>();
        calledStay = new List<GameObject>();
        calledExit = new List<GameObject>();
    }

    public void ClearCallList()
    {
        calledEnter.Clear();
        calledStay.Clear();
        calledExit.Clear();
    }

    public void CheckCollision(List<GameObject> list1, List<GameObject> list2 = null)
    {
        if (list2 == null)
        {
            list2 = list1;
        }
        foreach (var gameObject1 in list1)
        {
            if (gameObject1 is GameObjectList objList)
            {
                CheckCollision(list1, objList.Children);
            }

            foreach (var gameObject2 in list2)
            {
                if (gameObject1 is GameObjectList objList1)
                {
                    if (gameObject2 is GameObjectList objList2)
                    {
                        CheckCollision(objList1.Children, objList2.Children);
                    }
                    continue;
                }
                else if (gameObject2 is GameObjectList objList3)
                {
                    CheckCollision(list1, objList3.Children);
                    continue;
                }

                if (gameObject1 == gameObject2)
                {
                    continue;
                }

                if (!_collisions.ContainsKey(gameObject1))
                {
                    _collisions.Add(gameObject1, new List<GameObject>());
                }

                if (gameObject1.CollidesWith(gameObject2))
                {
                    if (_collisions.TryGetValue(gameObject1, out var list))
                    {
                        if (list.Contains(gameObject2))
                        {
                            if (!calledStay.Contains(gameObject1) && !calledEnter.Contains(gameObject1))
                            {
                                gameObject1.OnCollisionStay(gameObject2);
                                //calledStay.Add(gameObject1);
                            }
                        }
                        else
                        {
                            list.Add(gameObject2);
                            if (!calledEnter.Contains(gameObject1))
                            {
                                gameObject1.OnCollisionEnter(gameObject2);
                                //calledEnter.Add(gameObject1);
                            }
                        }
                    }
                }
                else
                {
                    if (_collisions.ContainsKey(gameObject1) && _collisions.TryGetValue(gameObject1, out var list))
                    {
                        if (list.Contains(gameObject2))
                        {
                            list.Remove(gameObject2);
                            if (!calledExit.Contains(gameObject1))
                            {
                                gameObject1.OnCollisionExit(gameObject2);
                                //calledExit.Add(gameObject1);
                            }
                        }
                    }
                }
            }
        }
    }
}

