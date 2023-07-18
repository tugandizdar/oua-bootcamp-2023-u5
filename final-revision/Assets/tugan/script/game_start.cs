using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.AI;
//using UnityEngine.InputSystem;

public class game_start : MonoBehaviour
{
    public GameObject prefab_player;
    public List<GameObject> prefab_environments;
    public GameObject prefab_gate;
    public GameObject prefab_zombie;
    public int max_zombie;

    private bool gates_opened;
    private string old_tile;
    
    // Start is called before the first frame update
    void Start()
    {
        System.Random rnd = new System.Random();

        GameObject environment = GameObject.Find("Environment");
        GameObject current_tile = Instantiate(prefab_environments[rnd.Next(prefab_environments.Count())], new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), environment.transform);
        current_tile.name = "tile-000-000";
        old_tile = current_tile.name;
        List<float> rotation_list = new List<float>() { 0f, 90f, 180f, 270f };
        current_tile.transform.Find("ground").transform.Rotate(0f, rotation_list[rnd.Next(rotation_list.Count())], 0f);


        create_zombie(current_tile.name);
        gates_opened = false;


        List<string> border_list = new List<string>() { "x+", "x-", "z+", "z-" };
        int gate_num = rnd.Next(border_list.Count());
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



        GameObject enemy = GameObject.Find("Enemy");
        if (enemy.transform.childCount == 0 && !gates_opened)
        {
            open_gates(current_tile);
            gates_opened = true;
        }
        if (gates_opened && current_tile != old_tile)
        {
            //Debug.Log(find_entered_gate(current_tile, old_tile));
            delete_gate(current_tile, find_entered_gate(current_tile, old_tile));
            create_zombie(current_tile);
            gates_opened = false;
        }
        old_tile = current_tile;




        string new_environment_name;
        string[] border_collection = new string[4] { "x+", "x-", "z+", "z-" };
        foreach (string border_name in border_collection)
        {
            new_environment_name = create_new_environment_name(current_tile, border_name);
            tile_list.Add(new_environment_name);
            if (GameObject.Find(current_tile + "/border/" + border_name + "/gate") && !GameObject.Find(new_environment_name))
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
        GameObject border = GameObject.Find(environment_name + "/border/" + border_name);

        GameObject gate = Instantiate(prefab_gate, border.transform.position, new Quaternion(0f, 0f, 0f, 0f), border.transform);
        gate.name = "gate";

    }

    void open_gates(string environment_name)
    {
        string[] border_collection = new string[4] { "x+", "x-", "z+", "z-" };
        foreach (string border_name in border_collection)
        {
            if (GameObject.Find(environment_name + "/border/" + border_name + "/gate"))
            {
                GameObject border = GameObject.Find(environment_name + "/border/" + border_name);
                border.transform.Find("+").localScale += new Vector3(0f, -0.1f, 0f);
                border.transform.Find("-").localScale += new Vector3(0f, -0.1f, 0f);

                string border_neighbour_name = border_name[0] + (border_name[1] == '+' ? "-" : "+");
                GameObject border_neighbour = GameObject.Find(create_new_environment_name(environment_name, border_name) + "/border/" + border_neighbour_name);
                border_neighbour.transform.Find("+").localScale += new Vector3(0f, -0.1f, 0f);
                border_neighbour.transform.Find("-").localScale += new Vector3(0f, -0.1f, 0f);

            }
        }
    }

    void delete_gate(string environment_name, string border_name)
    {
        GameObject border = GameObject.Find(environment_name + "/border/" + border_name);
        border.transform.Find("+").localScale += new Vector3(0f, 0.1f, 0f);
        border.transform.Find("-").localScale += new Vector3(0f, 0.1f, 0f);

        Destroy(border.transform.Find("gate").gameObject);
    }

    string find_entered_gate(string current_tile, string old_tile)
    {
        var current_location_arr = Regex.Matches(current_tile, @"[\+-](\d{3})")
            .Cast<Match>().Select(m => m.Value).ToArray();

        var old_location_arr = Regex.Matches(old_tile, @"[\+-](\d{3})")
            .Cast<Match>().Select(m => m.Value).ToArray();

        int x_diff = Convert.ToInt32(old_location_arr[0]) - Convert.ToInt32(current_location_arr[0]);
        int z_diff = Convert.ToInt32(old_location_arr[1]) - Convert.ToInt32(current_location_arr[1]);
        int gate_sign = x_diff == 0 ? z_diff : x_diff;
        
        return (x_diff == 0 ? "z" : "x") + (gate_sign == 1 ? "+" : "-");
    }

    void close_gates(string environment_name)
    {
        string[] border_collection = new string[4] { "x+", "x-", "z+", "z-" };
        foreach (string border_name in border_collection)
        {
            if (GameObject.Find(environment_name + "/border/" + border_name + "/gate"))
            {
                GameObject border = GameObject.Find(environment_name + "/border/" + border_name);
                border.transform.Find("+").localScale += new Vector3(0f, 0.1f, 0f);
                border.transform.Find("-").localScale += new Vector3(0f, 0.1f, 0f);
            }
        }
    }

    string find_tile_index(Vector3 player_position)
    {
        double axis_x = Math.Floor((player_position[0] + 15) / 30);
        string axis_x_str = Convert.ToString(Math.Abs(axis_x)).PadLeft(3, '0').PadLeft(4, (Math.Sign(axis_x) == 1 ? '+' : '-'));
        double axis_y = Math.Floor((player_position[2] + 15) / 30);
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

        Vector3 enviroment_location = new Vector3(Convert.ToInt32(enviroment_location_arr[0]) * 30f, 0f, Convert.ToInt32(enviroment_location_arr[1]) * 30f);

        System.Random rnd = new System.Random();

        GameObject environment = GameObject.Find("Environment");
        GameObject tile = Instantiate(prefab_environments[rnd.Next(prefab_environments.Count())], enviroment_location, new Quaternion(0f, 0f, 0f, 0f), environment.transform);
        tile.name = new_environment_name;

        List<float> rotation_list = new List<float>() { 0f, 90f, 180f, 270f };
        tile.transform.Find("ground").transform.Rotate(0f, rotation_list[rnd.Next(rotation_list.Count())], 0f);

        //Camera.main.transform.position = new Vector3(-10f, 10f, -10f) + new Vector3(Convert.ToInt32(enviroment_location_arr[0]) * 20f, 0f, Convert.ToInt32(enviroment_location_arr[1]) * 20f);

        //char opposite_sign = border_name[1] == '+' ? '-' : '+';
        string common_border = border_name[0] + (border_name[1] == '+' ? "-" : "+");
        create_gate(new_environment_name, common_border);

        List<string> border_list = new List<string>() { "x+", "x-", "z+", "z-" };
        border_list.Remove(common_border);

        int gate_num = rnd.Next(border_list.Count());
        for (int i = 0; i <= gate_num; i++)
        {
            int border_index = rnd.Next(border_list.Count());
            string border_chosen = border_list[border_index];
            border_list.Remove(border_chosen);
            create_gate(new_environment_name, border_chosen);
        }
    }

    void create_zombie(string environment_name)
    {
        GameObject enemy = GameObject.Find("Enemy");
        GameObject tile = GameObject.Find(environment_name);

        //Convert.ToInt32(tile.transform.Find("agent").GetChild(0).name);

        System.Random rnd = new System.Random();
        int zombie_num = rnd.Next(2, max_zombie);
        for (int i = 0; i < zombie_num; i++)
        {
            GameObject zombie = Instantiate(prefab_zombie, tile.transform.Find("ground/spawn_point").transform.position, new Quaternion(0f, 0f, 0f, 0f), enemy.transform);
            NavMeshAgent zombie_nav_mesh;
            zombie_nav_mesh = zombie.GetComponent<NavMeshAgent>();
            zombie_nav_mesh.agentTypeID = Convert.ToInt32(tile.transform.Find("agent").GetChild(0).name);
        }

        //Debug.Log(zombie_nav_mesh.agentTypeID);


        //zombie.GetComponent<UnityEngine.AI.NavMeshAgent>().agentTypeID = -334000983;
        //Instantiate(prefab_zombie, tile.transform.position + new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), enemy.transform);
    }

}
