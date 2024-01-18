using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static DialogConfig;

[CustomEditor(typeof(DialogConfig))]
[CanEditMultipleObjects]
public class DialogConfigEditor : Editor
{
    private DialogConfig _source;
    private GUIStyle _titleStyle;

    private void OnEnable()
    {
        _source = target as DialogConfig;
        _source.table ??= new();
    }

    #region INSPECTOR
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        InitStyle();
        DrawSpeakersDatabasePanel();

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(_source.speakerDatabases.Count == 0 || _source.speakerDatabases.Exists(x => x == null));
        DrawSpeakersPanel();

        DrawSentencePanel();

        EditorGUI.EndDisabledGroup();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(_source);
    }

    private void DrawSpeakersDatabasePanel()
    {
        EditorGUILayout.BeginVertical("box");

        DrawHeader();
        DrawBody();
        DrawFooter();

        EditorGUILayout.EndVertical();

        void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Speakers Database", _titleStyle);
            if (GUILayout.Button(new GUIContent("X", "Clear all Database"), GUILayout.Width(30)))
            {
                if (EditorUtility.DisplayDialog("Delete all database", "Do you want delete all speakers database?", "Yes", "No"))
                    _source.speakerDatabases.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }
        void DrawBody()
        {
            if (_source.speakerDatabases.Count != 0)
            {
                for (int i = 0; i < _source.speakerDatabases.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    _source.speakerDatabases[i] = EditorGUILayout.ObjectField(_source.speakerDatabases[i], typeof(SpeakerDatabase), false) as SpeakerDatabase;

                    if (GUILayout.Button(new GUIContent("X", "Remove database"), GUILayout.Width(30)))
                    {
                        _source.speakerDatabases.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        void DrawFooter()
        {
            if (GUILayout.Button(new GUIContent("Add new database", "")))
            {
                _source.speakerDatabases.Add(null);
            }
        }
    }

    private void DrawSpeakersPanel()
    {
        EditorGUILayout.BeginVertical("box");

        DrawHeader();
        DrawBody();
        DrawFooter();

        EditorGUILayout.EndVertical();

        void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Speakers", _titleStyle);
            if (GUILayout.Button(new GUIContent("X", "Clear all speakers"), GUILayout.Width(30)))
            {
                if (EditorUtility.DisplayDialog("Delete all speakers", "Do you want delete all speakers ?", "Yes", "No"))
                    _source.speakers.Clear();
            }

            EditorGUILayout.EndHorizontal();
        }
        void DrawBody()
        {
            if (_source.speakers.Count != 0)
            {
                for (int i = 0; i < _source.speakers.Count; i++)
                {
                    SpeakerConfig config = _source.speakers[i];

                    EditorGUILayout.BeginHorizontal();

                    if (_source.speakerDatabases.Count != 0)
                    {
                        if (_source.speakerDatabases.Count > 1)
                        {
                            List<string> alldatabaseLabel = new();
                            foreach (SpeakerDatabase sd in _source.speakerDatabases)
                                alldatabaseLabel.Add(sd?.name);

                            int idDatabate = _source.speakerDatabases.FindIndex(x => x == config.speakerDatabase);

                            idDatabate = EditorGUILayout.Popup(idDatabate < 0 ? 0 : idDatabate, alldatabaseLabel.ToArray());

                            config.speakerDatabase = _source.speakerDatabases[idDatabate];
                        }
                        else
                        {
                            config.speakerDatabase = _source.speakerDatabases.First();
                        }
                    }

                    if (config.speakerDatabase != null)
                    {
                        List<string> alldataLabel = new();
                        foreach (SpeakerData sd in config.speakerDatabase.speakerDatas)
                            alldataLabel.Add(sd?.label);

                        int idData = config.speakerDatabase.speakerDatas.FindIndex(x => x == config.speakerData);

                        idData = EditorGUILayout.Popup(idData < 0 ? 0 : idData, alldataLabel.ToArray());

                        config.speakerData = config.speakerDatabase.speakerDatas[idData];
                    }

                    config.position = (SpeakerConfig.POSITION)EditorGUILayout.EnumPopup(config.position);

                    if (GUILayout.Button(new GUIContent("X", "Remove speeker"), GUILayout.Width(30)))
                    {
                        _source.speakers.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();

                    _source.speakers[i] = config;
                }
            }
        }
        void DrawFooter()
        {
            if (GUILayout.Button(new GUIContent("Add new speaker", "")))
            {
                _source.speakers.Add(new DialogConfig.SpeakerConfig());
            }
        }
    }

    private void DrawSentencePanel()
    {
        EditorGUILayout.BeginVertical("box");

        DrawHeader();
        DrawBody();
        DrawFooter();

        EditorGUILayout.EndVertical();

        void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Sentence", _titleStyle);
            //if (GUILayout.Button(new GUIContent("X", "Clear all sentences"), GUILayout.Width(30)))
            //{
            //    if (EditorUtility.DisplayDialog("Delete all sentences", "Do you want delete all sentences ?", "Yes", "No"))
            //        _source.speakers.Clear();
            //}

            EditorGUILayout.EndHorizontal();
        }

        void DrawBody()
        {
            EditorGUILayout.BeginHorizontal();

            _source.csvDialog = (TextAsset)EditorGUILayout.ObjectField("File : ", _source.csvDialog, typeof(TextAsset), false);
            if (GUILayout.Button("Load"))
            {
                _source.table.Load(_source.csvDialog);
            }

            EditorGUILayout.EndHorizontal();

            if (!_source.table.IsLoaded())
                return;

            List<string> allRowLabel = new();

            if (_source.table.GetRowList().Count > 0)
            {
                foreach (var r in _source.table.GetRowList())
                    allRowLabel.Add(r.KEY);
            }


            for (int i = 0; i < _source.sentenceConfig.Count; i++)
            {
                var sentenceConfig = _source.sentenceConfig[i];

                int idRow = _source.table.GetRowList().FindIndex(x => x.KEY == sentenceConfig.key);

                EditorGUILayout.BeginHorizontal();

                idRow = EditorGUILayout.Popup(idRow < 0 ? 0 : idRow, allRowLabel.ToArray());

                sentenceConfig.key = allRowLabel[idRow];
                _source.sentenceConfig[i] = sentenceConfig;

                if (GUILayout.Button(new GUIContent("X", "Delete sentence"), GUILayout.Width(20)))
                    _source.sentenceConfig.RemoveAt(i);

                EditorGUILayout.EndHorizontal();
            }
        }

        void DrawFooter()
        {
            if (GUILayout.Button(new GUIContent("Add new sentence", "")))
                _source.sentenceConfig.Add(new(""));
        }
    }
    #endregion

    #region STYLE
    private void InitStyle()
    {
        _titleStyle = GUI.skin.label;
        _titleStyle.alignment = TextAnchor.MiddleCenter;
        _titleStyle.fontStyle = FontStyle.Bold;
    }
    #endregion
}
