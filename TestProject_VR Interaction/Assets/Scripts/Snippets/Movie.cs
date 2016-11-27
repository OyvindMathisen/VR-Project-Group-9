using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour
{

    private MovieTexture _movie;
    private bool _flag;
	void Awake ()
	{
	    _movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
	    _movie.loop = true;
        _movie.Play();
    }

    public void StopMovie()
    {
        _movie.Stop();
    }
}
