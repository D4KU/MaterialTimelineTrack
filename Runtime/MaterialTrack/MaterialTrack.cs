﻿using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(Material))]
[TrackColor(.5f, .84f, 1f)]
[TrackClipType(typeof(MaterialClip))]
public class MaterialTrack : TrackAsset, ILayerable
{
    /// <inheritdoc cref="ILayerable.CreateLayerMixer"/>
    public Playable CreateLayerMixer(
        PlayableGraph graph,
        GameObject go,
        int inputCount)
        => ScriptPlayable<MaterialLayerMixer>.Create(graph, inputCount);

    public override Playable CreateTrackMixer(
        PlayableGraph graph, GameObject go, int inputCount)
    {
        var mixer = ScriptPlayable<MaterialMixer>.Create(graph, inputCount);
        InitializeClips(mixer.GetBehaviour());
        return mixer;
    }

    void InitializeClips(IMaterialProvider provider)
    {
        foreach (TimelineClip clip in GetClips())
        {
            // Set display name of each clip
            var data = ((MaterialClip)clip.asset).template;
            clip.displayName = $"{data.propertyName} [{data.propertyType}]";

            // The track mixer created in this class is the object providing
            // each clip's behaviour access to the materials of the bound
            // renderer.
            data.provider = provider;
        }
    }
}
