package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
)

type Response struct {

}

func main() {
	client := http.Client{}
	contents, err := client.Get("https://localhost:5001/asd")
	if err != nil {
		log.Fatal(err.Error())
	}

	dst := new(bytes.Buffer)

	var body, _ = ioutil.ReadAll(contents.Body)
	json.Indent(dst, body, "", "  ")
	fmt.Printf("Result: %s\n", dst)
}
