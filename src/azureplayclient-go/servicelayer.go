package main

import (
	"bytes"
	"encoding/json"
	"log"
)

type LearningResource struct {
	Id string
	ServiceId string
	Name string
	Uri string
}

func getServices(client *SimpleREST) *bytes.Buffer {
	buffer, err := client.jsonRequest("services")
	if err != nil {
		log.Fatal(err.Error())
	}
	return buffer
}

func newLearningResource(name string, serviceId string, uri string) []byte {
	re := LearningResource{Name:name, ServiceId: serviceId, Uri:uri, }
	result, err := json.Marshal(re)
	if err != nil {
		log.Fatal(err.Error())
	}
	return result
}

func createLearningResource(client *SimpleREST, resource []byte) string {
	res, err := client.jsonPost("LearningResources", resource)
	if err != nil {
		log.Fatal("Post failed")
	}
	return res
}

