package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"log"
	"net/url"
	"crypto/rand"
)

type LearningResource struct {
	Id string
	ServiceId string
	Name string
	Uri string
}

func getServices(client *SimpleREST) *bytes.Buffer {
	buffer, err := client.jsonRequest("modernsqlservices")
	if err != nil {
		log.Fatal(err.Error())
	}
	return buffer
}

func findServices(client *SimpleREST, searchQuery string) *bytes.Buffer {
	query := url.PathEscape(searchQuery) // TODO sanitize user input
	url := "modernsqlservices/find/" + query
	buffer, err := client.jsonRequest(url)
	if err != nil {
		log.Fatal(err.Error())
	}
	return buffer
}

func getLearningResources(client *SimpleREST) *bytes.Buffer {
	buffer, err := client.jsonRequest("modernsqlLearningResources")
	if err != nil {
		log.Fatal(err.Error())
	}
	return buffer
}

func createLearningResource(client *SimpleREST, resource []byte) string {
	res, err := client.jsonPost("modernsqlLearningResources", resource)
	if err != nil {
		log.Fatalf("Post failed %v", err.Error())
	}
	return res
}

func findLearningResource(client *SimpleREST, searchQuery string) *bytes.Buffer {
	query := url.PathEscape(searchQuery) // TODO sanitize user input
	url := "modernsqlLearningResources/find/" + query
	buffer, err := client.jsonRequest(url)
	if err != nil {
		log.Fatal(err.Error())
	}
	return buffer
}

func newLearningResource(name string, serviceId string, uri string) []byte {
	re := LearningResource{Id: makeguid(), Name:name, ServiceId: serviceId, Uri:uri, }
	//re := LearningResource{Name:name, ServiceId: serviceId, Uri:uri, }
	result, err := json.Marshal(re)
	if err != nil {
		log.Fatal(err.Error())
	}
	return result
}

func makeguid() (string) {
	b := make([]byte, 16)
	_, err := rand.Read(b)
	if err != nil {
		log.Fatal(err)
	}
	uuid := fmt.Sprintf("%x-%x-%x-%x-%x",
		b[0:4], b[4:6], b[6:8], b[8:10], b[10:])
	return uuid
}