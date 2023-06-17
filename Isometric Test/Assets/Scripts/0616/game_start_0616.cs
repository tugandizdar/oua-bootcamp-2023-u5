using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.InputSystem;

public class game_start_0616 : MonoBehaviour
{
    public GameObject prefab_player;
    public GameObject prefab_environment;
    public GameObject prefab_gate;
    // Start is called before the first frame update
    void Start()
    {
        GameObject environment = GameObject.Find("Environment");
        GameObject current_tile = Instantiate(prefab_environment, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), environment.transform);
        current_tile.name = "tile-000-000";

        List<string> border_list = new List<string>() { "x+", "x-", "z+", "z-" };
        System.Random rnd = new System.Random();
        int gate_num = rnd.Next(0, border_list.Count());
        for (int i = 0; i <= gate_num; i++)
        {
            int border_index = rnd.Next(border_list.Count());
            string border_chosen = border_list[border_index];
            border_list.Remove(border_chosen);
            create_gate("tile-000-000", border_chosen);
        }




    }


    // Update is called once per frame
    void Update()
    {


        List<string> tile_list = new List<string>();

        string current_tile = find_tile_index(prefab_player.transform.localPosition);
        tile_list.Add(current_tile);

        string new_environment_name;
        string[] border_collection = new string[4] { "x+", "x-", "z+", "z-" };
        foreach (string border_name in border_collection)
        {
            new_environment_name = create_new_environment_name(current_tile, border_name);
            tile_list.Add(new_environment_name);
            if (GameObject.Find(current_tile + "/ground/" + border_name + "/gate") && !GameObject.Find(new_environment_name))
            {
                create_new_environment(new_environment_name, border_name);
            }
        }

        GameObject environment = GameObject.Find("Environment");
        foreach (Transform child in environment.transform)
        {
            if (!tile_list.Any(child.name.Contains))
                {
                Destroy(environment.transform.Find(child.name).gameObject);
                }
        }

    }

    void create_gate(string environment_name, string border_name)
    {
        GameObject border = GameObject.Find(environment_name + "/ground/" + border_name);
        if (border_name[0] == 'x')
        {
            border.transform.Find("+").localPosition += new Vector3(0f, 0f, 0.25f);
            border.transform.Find("+").localScale += new Vector3(-0.5f, 0f, 0f);
            border.transform.Find("-").localPosition += new Vector3(0f, 0f, -0.25f);
            border.transform.Find("-").localScale += new Vector3(-0.5f, 0f, 0f);

            GameObject gate = Instantiate(prefab_gate,
                border.transform.position + new Vector3(0f, 1f, 0f),
                Quaternion.Euler(new Vector3(0f, -90f + float.Parse(border_name[1] + "90"), 0f)),
                border.transform);
            gate.name = "gate";
        }
        else if (border_name[0] == 'z')
        {
            border.transform.Find("+").localPosition += new Vector3(0.25f, 0f, 0f);
            border.transform.Find("+").localScale += new Vector3(-0.5f, 0f, 0f);
            border.transform.Find("-").localPosition += new Vector3(-0.25f, 0f, 0f);
            border.transform.Find("-").localScale += new Vector3(-0.5f, 0f, 0f);

            GameObject gate = Instantiate(prefab_gate,
                border.transform.position + new Vector3(0f, 1f, 0f),
                Quaternion.Euler(new Vector3(0f, 180f + float.Parse(border_name[1] + "90"), 0f)),
                border.transform);
            gate.name = "gate";

        }
    }


    string find_tile_index(Vector3 player_position)
    {
        double axis_x = Math.Floor((player_position[0] + 10) / 20);
        string axis_x_str = Convert.ToString(Math.Abs(axis_x)).PadLeft(3, '0').PadLeft(4, (Math.Sign(axis_x) == 1 ? '+' : '-'));
        double axis_y = Math.Floor((player_position[2] + 10) / 20);
        string axis_y_str = Convert.ToString(Math.Abs(axis_y)).PadLeft(3, '0').PadLeft(4, (Math.Sign(axis_y) == 1 ? '+' : '-'));

        return "tile" + axis_x_str + axis_y_str;
    }

    string create_new_environment_name(string old_environment_name, string border_name)
    {
        var enviroment_location_arr = Regex.Matches(old_environment_name, @"[\+-](\d{3})")
            .Cast<Match>().Select(m => m.Value).ToArray();

        int axis = border_name[0] == 'x' ? 0 : 1;
        //int direction = border_name[1] == '+' ? +1 : -1;
        //int axis_location = Convert.ToInt32(enviroment_location_arr[axis]) + direction;
        int axis_location = Convert.ToInt32(enviroment_location_arr[axis]) + (border_name[1] == '+' ? +1 : -1);
        //char axis_location_sign = Math.Sign(axis_location) == 1 ? '+' : '-';
        //enviroment_location_arr[axis] = Convert.ToString(Math.Abs(axis_location)).PadLeft(3, '0').PadLeft(4, axis_location_sign);
        enviroment_location_arr[axis] = Convert.ToString(Math.Abs(axis_location)).PadLeft(3, '0').PadLeft(4, (Math.Sign(axis_location) == 1 ? '+' : '-'));

        return Regex.Replace(old_environment_name, @"[\+-](\d{3})[\+-](\d{3})", enviroment_location_arr[0] + enviroment_location_arr[1]);
    }

    void create_new_environment(string new_environment_name, string border_name)
    {
        var enviroment_location_arr = Regex.Matches(new_environment_name, @"[\+-](\d{3})")
            .Cast<Match>().Select(m => m.Value).ToArray();

        Vector3 enviroment_location = new Vector3(Convert.ToInt32(enviroment_location_arr[0]) * 20f, 0f, Convert.ToInt32(enviroment_location_arr[1]) * 20f);


        GameObject environment = GameObject.Find("Environment");
        GameObject tile = Instantiate(prefab_environment, enviroment_location, new Quaternion(0f, 0f, 0f, 0f), environment.transform);
        tile.name = new_environment_name;

        //Camera.main.transform.position = new Vector3(-10f, 10f, -10f) + new Vector3(Convert.ToInt32(enviroment_location_arr[0]) * 20f, 0f, Convert.ToInt32(enviroment_location_arr[1]) * 20f);

        //char opposite_sign = border_name[1] == '+' ? '-' : '+';
        string common_border = border_name[0] + (border_name[1] == '+' ? "-" : "+");
        create_gate(new_environment_name, common_border);

        List<string> border_list = new List<string>() { "x+", "x-", "z+", "z-" };
        border_list.Remove(common_border);

        System.Random rnd = new System.Random();
        int gate_num = rnd.Next(0, border_list.Count());
        for (int i = 0; i <= gate_num; i++)
        {
            int border_index = rnd.Next(border_list.Count());
            string border_chosen = border_list[border_index];
            border_list.Remove(border_chosen);
            create_gate(new_environment_name, border_chosen);
        }
    }



}
