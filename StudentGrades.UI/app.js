const apiUrl = "https://localhost:7197/api";

let editingSubjectId = null;

const subjectsSection = document.getElementById("subjectsSection");
const studentsSection = document.getElementById("studentsSection");
const resultsSection = document.getElementById("resultsSection");

document.getElementById("subjectsBtn").addEventListener("click", () => {
    subjectsSection.style.display = "block";
    studentsSection.style.display = "none";
    resultsSection.style.display = "none";
});

document.getElementById("studentsBtn").addEventListener("click", () => {
    subjectsSection.style.display = "none";
    studentsSection.style.display = "block";
    resultsSection.style.display = "none";

    loadSubjectDropdown();
    loadStudents();
});

document.getElementById("resultsBtn").addEventListener("click", () => {
    subjectsSection.style.display = "none";
    studentsSection.style.display = "none";
    resultsSection.style.display = "block";

    loadResults();
});

async function loadSubjects() {

    const response = await fetch(`${apiUrl}/subjects`);

    const subjects = await response.json();

    const table = document.querySelector("#subjectsTable tbody");

    table.innerHTML = "";

    subjects.forEach(subject => {

        table.innerHTML += `
            <tr>
                <td>${subject.subjectId}</td>
                <td>${subject.subjectName}</td>
                <td>
                    <button onclick="editSubject(${subject.subjectId}, '${subject.subjectName}')">Edit</button>
                    <button onclick="deleteSubject(${subject.subjectId})">Delete</button>
                </td>
            </tr>
        `;

    });

}

loadSubjects();

async function addSubject() {

    const subjectName = document.getElementById("subjectName").value;

    if (subjectName === "") {
        alert("Enter subject name");
        return;
    }

    if (editingSubjectId === null) {

        // Add Subject
        await fetch(`${apiUrl}/subjects`, {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                subjectName: subjectName
            })

        });

    } else {

        // Update Subject
        await fetch(`${apiUrl}/subjects/${editingSubjectId}`, {

            method: "PUT",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify({
                subjectId: editingSubjectId,
                subjectName: subjectName
            })

        });

        editingSubjectId = null;
        document.getElementById("saveSubjectBtn").textContent = "Save Subject";
    }

    document.getElementById("subjectName").value = "";

    loadSubjects();
}

document
    .getElementById("saveSubjectBtn")
    .addEventListener("click", addSubject);

function editSubject(id, name) {

    editingSubjectId = id;

    document.getElementById("subjectName").value = name;

    document.getElementById("saveSubjectBtn").textContent = "Update Subject";
}

async function deleteSubject(id) {

    if (!confirm("Delete this subject?"))
        return;

    await fetch(`${apiUrl}/subjects/${id}`, {

        method: "DELETE"

    });

    loadSubjects();

}

async function loadSubjectDropdown() {

    const response = await fetch(`${apiUrl}/subjects`);

    const subjects = await response.json();

    const dropdown = document.getElementById("subjectSelect");

    dropdown.innerHTML = "";

    subjects.forEach(subject => {

        dropdown.innerHTML += `
            <option value="${subject.subjectId}">
                ${subject.subjectName}
            </option>
        `;

    });

}

async function addStudent() {

    const studentName = document.getElementById("studentName").value;
    const subjectId = document.getElementById("subjectSelect").value;
    const grade = document.getElementById("grade").value;

    if (studentName === "" || grade === "") {
        alert("Please fill all fields.");
        return;
    }

    await fetch(`${apiUrl}/students`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            studentName: studentName,
            subjectId: Number(subjectId),
            grade: Number(grade)
        })
    });

    document.getElementById("studentName").value = "";
    document.getElementById("grade").value = "";

    loadStudents();
}

document
    .getElementById("saveStudentBtn")
    .addEventListener("click", addStudent);

async function loadStudents() {

    const response = await fetch(`${apiUrl}/students`);

    const students = await response.json();

    const table = document.querySelector("#studentsTable tbody");

    table.innerHTML = "";

    students.forEach(student => {

        table.innerHTML += `
            <tr>
                <td>${student.studentName}</td>
                <td>${student.subjectName}</td>
                <td>${student.grade}</td>
                <td>
                    <button onclick="deleteStudent(${student.studentId})">
                        Delete
                    </button>
                </td>
            </tr>
        `;

    });

}

async function deleteStudent(id) {

    if (!confirm("Delete this student?"))
        return;

    await fetch(`${apiUrl}/students/${id}`, {
        method: "DELETE"
    });

    loadStudents();
}

async function loadResults(search = "", remarks = "") {

    let url = `${apiUrl}/students`;

    const params = [];

    if (search) {
        params.push(`search=${encodeURIComponent(search)}`);
    }

    if (remarks) {
        params.push(`remarks=${remarks}`);
    }

    if (params.length > 0) {
        url += "?" + params.join("&");
    }

    const response = await fetch(url);

    const students = await response.json();

    const table = document.querySelector("#resultsTable tbody");

    table.innerHTML = "";

    students.forEach(student => {

        table.innerHTML += `
            <tr>
                <td>${student.studentName}</td>
                <td>${student.subjectName}</td>
                <td>${student.grade}</td>
                <td class="${student.remarks === "PASS" ? "pass" : "fail"}">
                    ${student.remarks}
                </td>
            </tr>
        `;

    });

}

document
    .getElementById("searchStudent")
    .addEventListener("input", function () {

        loadResults(
            this.value,
            document.getElementById("filterRemarks").value
        );

    });

document
    .getElementById("filterRemarks")
    .addEventListener("change", function () {

        loadResults(
            document.getElementById("searchStudent").value,
            this.value
        );

    });
