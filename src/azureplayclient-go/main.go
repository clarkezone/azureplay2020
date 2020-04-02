package main

import (
	"flag"
	"fmt"
	"net/http"
	"os"
)

func main() {
	lrListRequested, lrFind, lrCreate, lrCreateName, lrCreateServiceId, lrCreateUri, sdListRequested, sdFind := parseCmdline()

	client := &http.Client{}

	// REST endpoint
	const s = "https://localhost:5001/"
	rest := CreateSimpleRest(s, client)

	if *lrListRequested == true {
		// TODO
	} else if *lrFind != "" {
		// TODO
	} else if *lrCreate == true {
		re := newLearningResource(*lrCreateName, *lrCreateServiceId, *lrCreateUri)
		dst := createLearningResource(rest, re)
		fmt.Printf("Create Result: %s\n", dst)
	} else if *sdListRequested == true {
		dst := getServices(rest)
		fmt.Printf("Result: %s\n", dst)
	} else if *sdFind != "" {
		dst := findServices(rest, *sdFind)
		fmt.Printf("Result: %s\n", dst)
		//TODO2 list by category
	}
}

func parseCmdline() (*bool, *string, *bool, *string, *string, *string, *bool, *string) {
	lrCommand := flag.NewFlagSet("LearningResource", flag.ContinueOnError)
	sdCommand := flag.NewFlagSet("ServiceDescription", flag.ContinueOnError)

	lrListRequested := lrCommand.Bool("list", false, "list all learning resources")
	lrFind := lrCommand.String("find", "", "resource name to search for")
	lrCreate := lrCommand.Bool("create", false, "create a new resource")
	lrCreateName := lrCommand.String("name", "", "Name of resource")
	lrCreateServiceId := lrCommand.String("serviceid", "", "ID of service")
	lrCreateUri := lrCommand.String("uri", "", "URI for resource")

	sdListRequested := sdCommand.Bool("list", false, "list all services")
	sdFind := sdCommand.String("find", "", "service name to search for")

	if len(os.Args) < 2 {
		fmt.Println("LearningResource or ServiceDescription subcommand is required")
		os.Exit(1)
	}
	switch os.Args[1] {
	case "LearningResource":
		err := lrCommand.Parse(os.Args[2:])
		if err != nil || (*lrListRequested == false && *lrFind == "" && *lrCreate == false) {
			lrCommand.PrintDefaults()
			os.Exit(1)
		}

	case "ServiceDescription":
		err := sdCommand.Parse(os.Args[2:])
		if err != nil || (*sdListRequested == false && *sdFind == "") {
			sdCommand.PrintDefaults()
			os.Exit(1)
		}
	default:
		flag.PrintDefaults()
		os.Exit(1)
	}
	return lrListRequested, lrFind, lrCreate, lrCreateName, lrCreateServiceId, lrCreateUri, sdListRequested, sdFind
}

