/**
 * Aadpté de 
 * LAGUE S, [Unity] 2D Curve Editor (E01 : introduction and concepts), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=RF04Fi9OCPc)
 * LAGUE S, [Unity] 2D Curve Editor (E02 : adding and moving points), télévsersée le 25 janvier 2018 (https://www.youtube.com/watch?v=n_RHttAaRCk) 
 * LAGUE S, [Unity] 2D Curve Editor (E03 : closed path and auto-controls), télévsersée le 21 janvier 2018 (https://www.youtube.com/watch?v=nNmFLWup4_k) 
 * 
 * Alexandre Ouellet - 9 octobre 2022
 */
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages available paths in the game. Handles the unicity of path's names.
/// </summary>
public class PathManager : MonoBehaviour
{
    // Pseudo-singleton implementation
    public static PathManager Instance { get; private set; }

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    [HideInInspector]
    public Path path;                           // Current path in the editor

    public float level;                         // Y-Level on which anchor points are added

    [HideInInspector, SerializeField]
    public List<Path> pathCollection;           // List of all created paths

    private int internalIndex = 0;              // Internal index for autonaming path.

    /// <summary>
    /// Creates a new path and add it to the collection. Name is automatically generated
    /// </summary>
    public void CreatePath()
    {
        path = new Path(transform.position, GetAvailableName());
        pathCollection.Add(path);
    }

    /// <summary>
    /// Returns a generic path of the format "Path #" that is not already taken.
    /// </summary>
    /// <returns>A new unique name.</returns>
    private string GetAvailableName()
    {
        int index = internalIndex;
        while(pathCollection.Any(p => p.Name.Equals($"Path {index}"))) {
            index++;
        }
        internalIndex = index++;
        return $"Path {index}";
    }

    /// <summary>
    /// Changes the name of the path given by pathIndex. If name is already taken, then 
    /// no change is performed.
    /// </summary>
    /// <param name="pathIndex">The index of the path for which to change the name.</param>
    /// <param name="newName">The new name of the path.</param>
    /// <returns>True if the name has been changed, false if the name is already taken and therefore
    /// could not be changed.</returns>
    public bool Rename(int pathIndex, string newName)
    {
        if(pathCollection.Any(p => p.Name.Equals(newName)))
        {
            return false;
        }

        pathCollection[pathIndex].Name = newName;
        return true;
    }

    /// <summary>
    /// Returns the path with the given name.
    /// </summary>
    /// <param name="name">The name of the path to retreive.</param>
    /// <returns>The path with the given name or null reference if such path could not
    /// be found.</returns>
    public Path GetPathByName(string name)
    {
        foreach(Path path in pathCollection)
        {
            if(path.Name.Equals(name))
            {
                return path;
            }
        }

        return null;
    }
}
